const BASE_URL = 'https://api.themoviedb.org/3';

const MOCKED_AXIOS_ERROR = {
  isAxiosError: true,
  response: {
    data: {
      status_code: 7,
      status_message: 'Invalid API key: You must be granted a valid key.',
      success: false,
    },
  },
};

export { BASE_URL, MOCKED_AXIOS_ERROR };
