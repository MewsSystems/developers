# Test Runner

**Date: 2024-04-21**

## Context

We need to install a test runner in order to implement automated testing strategy. Jest and Vite have compatibility issues that have led to inefficiencies and complexities in our testing process. To address these challenges, we are considering adopting Vitest as our test runner.

## Decision

After careful consideration, we have decided to use Vitest as our test runner for the following reasons:

1. **Compatibility with Vite:** Vite and Vitest are designed to work seamlessly together. Unlike Jest, which was not initially built to integrate with Vite, Vitest is tailored to take full advantage of Vite's development server and features. This compatibility will result in a more streamlined and efficient testing process.

2. **Improved Speed:** Vite's fast development server is known for its exceptional speed. By using Vitest with Vite, we can take advantage of this speed, resulting in quicker test runs. This improved speed is crucial for maintaining a fast development feedback loop.

3. **Easier Configuration:** Vitest's configuration is designed to be straightforward and aligned with Vite's configuration style. This makes it easier to set up and maintain our testing environment, reducing the complexity of our development workflow.

4. **Compatibility with Testing Libraries:** Vitest has compatibility with the `@testing-library/react` library, which we have chosen as our primary tool for testing. This compatibility ensures that our existing testing infrastructure and practices can seamlessly transition to Vitest, minimizing disruptions in our testing process.

5. **Innovation Token:** We recognize that making this decision will require some time and effort. Therefore, we plan to spend one of our innovation tokens to address this challenge. The advantages of using Vitest with Vite, along with the issues we've encountered with Jest, make this investment worthwhile.

## Conclusion

Choosing Vitest as our test runner aligns with our goal of improving the compatibility between our development tools and streamlining our testing process. By leveraging Vitest's compatibility with Vite and the `@testing-library/react` library, we can achieve faster test runs, a more efficient testing environment, and maintain our preferred testing practices.

This decision will contribute to a more effective and enjoyable development experience, ultimately leading to higher productivity and code quality.

## Additional Resources

- [Choose Boring Technology - Innovation tokens](https://boringtechnology.club/#17)
- [Vitest Documentation](https://vitest.dev/)
- [Vite Documentation](https://vitejs.dev/)
- [Testing Library](https://testing-library.com/)
