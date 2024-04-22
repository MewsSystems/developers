# Project File Structure

**Date: 2024-04-21**

## Context

Our project requires a well-organized file structure to maintain consistency and facilitate collaboration among team members. We have defined a file structure with specific folder names and file naming conventions based on the Angular style guide.

## Decision

### Folder structure

We have chosen the following file structure for our project:

```
project-root/
├── app/
   ├── components/
   ├── pages/
        ├── ExampleView/
            ├── components/
            ├── pages/
            ├── services/
   ├── services/
├── design-system/
├── data/
├── testing-utils/
```

- **app:** This is the main folder containing the core structure of our application. It can be replicated at various depth levels to represent different parts of the app. Each replicated folder can have its own set of components, pages, and services.
  - **components:** Contains common components used within this level of the app. These are typically reusable UI elements like buttons, forms, and other shared parts.
  - **pages:** Contains the route-based files for this level, usually representing individual pages or screens in the app. These might contain unique components or a combination of shared components.
  - **service:** Holds common singletons that are used across this level, such as global context providers, software development kits (SDKs), or other shared resources that might need to maintain state or connect with external services.
- **design-system:** Shadcdn/ui components
- **data:** Contains types, API functions, and helper functions related to business entities.
- **testing-utils:** Helper functions for tests and fixtures are organized in this folder to assist in testing our components and services.

We also try to follow [the principle of code colocation](https://kentcdodds.com/blog/colocation) as much as possible, making the code mimic the tree structure of the app.

### File Naming Convention

We follow a naming convention where file extensions indicate the type of content within the file. This convention helps team members quickly identify the purpose of each file. It's heavily inspired on the [Angular style guide file naming](https://angular.io/guide/styleguide#style-02-01)

The extensions we propose are the following (along with their purpose).

- `.page`: Root view component of a specific route
- `.utils`: General helper functions, react hooks, etc
- `.data`: Data layer communication functions (API, hooks that retrieve data, etc)
- `.types`: Typescript types
- `.spec`: Tests files

## Conclusion

This file structure, along with the naming conventions, will help us maintain a clean and organized codebase, making it easier for team members to navigate and collaborate on the project. It also promotes consistency and clarity in our project's organization.

## Additional Resources

- [Code colocation](https://kentcdodds.com/blog/colocation)
- [Angular Style Guide](https://angular.io/guide/styleguide)
