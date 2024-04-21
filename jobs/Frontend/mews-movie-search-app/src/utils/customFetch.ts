export const customFetch = async <returnType>(
  endpoint: string
): Promise<returnType> => {
  const response = await fetch(endpoint);
  if (!response.ok) {
    throw new Error("Error");
  }
  const data = await response.json();
  return data;
};
