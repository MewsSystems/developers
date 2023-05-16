public class ServiceLocator {
	private static Map<Type, Object> mockLookup = new Map<Type, Object>();

    @TestVisible
    private static void setMock(Type serviceType, Object serviceImpl) {
        mockLookup.put(serviceType, serviceImpl);
    }

    public static Object getInstance(Type serviceType) {
        if (mockLookup.containsKey(serviceType)) {
            return mockLookup.get(serviceType);
        }
        return serviceType.newInstance();
    }
}