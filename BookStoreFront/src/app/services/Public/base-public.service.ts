import { HttpClient, HttpParams } from '@angular/common/http';
import {
  DestroyRef,
  inject,
  signal,
  computed,
  WritableSignal,
} from '@angular/core';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { ApiResponse } from '../../models/apiResponse';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { catchError, EMPTY, map, merge, Subject, switchMap } from 'rxjs';

export abstract class BasePublicService<
  TDetails,
  TListItem,
  TListResponse,
  TPagination
> {
  protected http = inject(HttpClient);
  protected alertService = inject(AlertService);
  protected errorHandler = inject(ErrorHandlerService);
  protected destroyRef = inject(DestroyRef);

  protected abstract readonly apiUrl: string;

  // Signals for state management
  protected detailsSig = signal<TDetails | null>(null);
  protected itemsSig = signal<TListItem[]>([]);
  protected paginationSig: WritableSignal<TPagination>;

  // Internal signals for reactive listing
  protected pageNumberSig = signal<number>(1);
  protected pageSizeSig = signal<number>(20);
  protected refreshTrigger$ = new Subject<void>();

  // Publicly exposed signals (read-only)
  details = computed(() => this.detailsSig());
  items = computed(() => this.itemsSig());
  pagination = computed(() => this.paginationSig());

  constructor(initialPagination: TPagination) {
    this.paginationSig = signal<TPagination>(initialPagination);
  }

  /**
   * Setup a reactive pipeline for data listing.
   * This should be called in the constructor of derived classes if they support pagination.
   */
  protected setupListingLogic() {
    const params$ = toObservable(
      computed(() => ({
        page: this.pageNumberSig(),
        size: this.pageSizeSig(),
      }))
    );

    const refresh$ = this.refreshTrigger$.pipe(
      map(() => ({
        page: this.pageNumberSig(),
        size: this.pageSizeSig(),
      }))
    );

    merge(params$, refresh$)
      .pipe(
        switchMap(({ page, size }) => {
          let params = new HttpParams()
            .set('PageNumber', page.toString())
            .set('PageSize', size.toString());

          // Merge with extra params from derived class
          const extraParams = this.getExtraParams();
          extraParams.keys().forEach((key) => {
            const value = extraParams.get(key);
            if (value !== null) {
              params = params.set(key, value);
            }
          });

          return this.http
            .get<ApiResponse<TListResponse>>(`${this.apiUrl}`, { params })
            .pipe(
              catchError((err) => {
                this.errorHandler.handleError(err);
                return EMPTY;
              })
            );
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe((response) => {
        if (response.data) {
          const items = this.getItemsFromResponse(response.data);
          const pagination = this.getPaginationFromResponse(response.data);
          if (items) this.itemsSig.set([...items]);
          if (pagination) this.paginationSig.set(pagination);
        }
      });
  }

  /**
   * Hook for derived classes to provide additional query parameters.
   */
  protected getExtraParams(): HttpParams {
    return new HttpParams();
  }

  /**
   * Abstract methods to parse items and pagination from response.
   * Only needed if setupListingLogic is used.
   */
  protected getItemsFromResponse(
    response: TListResponse
  ): TListItem[] | undefined {
    return undefined;
  }
  protected getPaginationFromResponse(
    response: TListResponse
  ): TPagination | undefined {
    return undefined;
  }

  /**
   * Method for fetching details by ID or slug.
   */
  getDetails(identifier: string | number, extraParams?: HttpParams) {
    this.http
      .get<ApiResponse<TDetails>>(`${this.apiUrl}/${identifier}`, {
        params: extraParams,
      })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          if (res.data) {
            this.detailsSig.set(res.data);
          }
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  /**
   * Set list parameters to trigger reactive fetch.
   */
  getAll(pageNumber: number = 1, pageSize: number = 20) {
    this.pageNumberSig.set(pageNumber);
    this.pageSizeSig.set(pageSize);
  }

  /**
   * Manually trigger a refresh of the current listing
   */
  refresh() {
    this.refreshTrigger$.next();
  }
}
