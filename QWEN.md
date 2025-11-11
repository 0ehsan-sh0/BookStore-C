This is a full stack application that uses Angular as the frontend and .NET Core as the backend.

## Technologies

Frontend:

- Angular
- DaisyUI
- TailwindCSS

- "dependencies": {
    "@angular/animations": "^19.2.14",
    "@angular/common": "^19.2.0",
    "@angular/compiler": "^19.2.0",
    "@angular/core": "^19.2.0",
    "@angular/forms": "^19.2.0",
    "@angular/platform-browser": "^19.2.0",
    "@angular/platform-browser-dynamic": "^19.2.0",
    "@angular/router": "^19.2.0",
    "@tailwindcss/postcss": "^4.1.12",
    "daisyui": "^5.0.50",
    "jalaali-js": "^1.2.8",
    "lucide-angular": "^0.539.0",
    "postcss": "^8.5.6",
    "rxjs": "~7.8.0",
    "swiper": "^8.4.7",
    "tailwindcss": "^4.1.12",
    "tslib": "^2.3.0",
    "zone.js": "~0.15.0"
  }

-  "devDependencies": {
    "@angular-devkit/build-angular": "^19.2.1",
    "@angular/cli": "^19.2.1",
    "@angular/compiler-cli": "^19.2.0",
    "@types/jasmine": "~5.1.0",
    "jasmine-core": "~5.6.0",
    "karma": "~6.4.0",
    "karma-chrome-launcher": "~3.2.0",
    "karma-coverage": "~2.2.0",
    "karma-jasmine": "~5.1.0",
    "karma-jasmine-html-reporter": "~2.1.0",
    "typescript": "~5.7.2"
  }

Backend:

- .NET Core
- Dapper
- ImageSharp
- JWT

## Guidelines
- Project is RTL and the language is Persian.
- consider using daisy ui and tailwind css for the frontend and in the backend consider DI and use dependency injection for repos
- in the backend all DTOs are in the Request handler folder and they separate to :
Admin,Auth,Public
Then in each we have :
Mappers,QueryObjects,Requests,Responses
remeber if any of them you want to add, add it in the right folder with right file

- foregin services are in the services folder in the backend if you want to add any put it there
- business logic layer folder is the middle layer between data access and controllers logic goes there
- for front if you need to make any model to sync with the backend etc put it in the models folder then use it
- don't forget in the front use lucid angular for icons it is installed on the project
- do not forget to add using of a file you added in the backend