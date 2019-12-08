export const mockIndexedDB = () => {
    window.indexedDB = {};
    const indexedDBmock = {
        openCursorAction: {}
    };
    window.indexedDB.mock = indexedDBmock;
    indexedDBmock.objectStore = {
        openCursor: jest.fn().mockReturnValue(window.indexedDB.mock.openCursorAction),
        put: jest.fn(),
        createIndex: jest.fn()
    };
    indexedDBmock.objectStoreFactory = {
        objectStore: jest.fn().mockReturnValue(window.indexedDB.mock.objectStore)
    };
    indexedDBmock.openRequest = {
        // result is db
        result: {
            createObjectStore: jest.fn().mockReturnValue(indexedDBmock.objectStore),
            transaction: jest.fn().mockReturnValue(window.indexedDB.mock.objectStoreFactory)
        }
    };
    window.indexedDB.open = jest.fn().mockReturnValue(window.indexedDB.mock.openRequest);
};
