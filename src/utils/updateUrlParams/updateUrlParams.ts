type UpdateUrlProps = {
  params: URLSearchParams;
  key: string;
  value: string | null;
};

export const updateUrlParams = ({params, key, value}: UpdateUrlProps) => {
  const newParams = new URLSearchParams(params);

  if (value) {
    newParams.set(key, value);
  } else {
    newParams.delete(key);
  }
  return newParams;
};
