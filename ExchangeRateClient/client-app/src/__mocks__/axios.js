// src/__mocks__/axios.ts

const mockAxios = jest.genMockFromModule("axios");

// this is the key to fix the axios.create() undefined error!
mockAxios.create = jest.fn(() => mockAxios);

export default mockAxios;
