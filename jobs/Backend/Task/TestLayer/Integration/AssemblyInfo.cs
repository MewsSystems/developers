using Xunit;

// Disable test parallelization to prevent database cleanup conflicts
// Integration tests use a shared database and Respawn for cleanup
// Running tests in parallel can cause race conditions during database cleanup
[assembly: CollectionBehavior(DisableTestParallelization = true)]
