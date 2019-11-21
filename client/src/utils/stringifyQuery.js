export default (query) => {
  if (!query) return '';
  const keys = Object.keys (query);

  const parameters = keys.map (name => {
    const key = encodeURIComponent (String(name).trim());
    const value = encodeURIComponent (String(query[name]).trim());
    const validated = key.length > 0 && value.length > 0;

    if (!validated) return '';
    
    return `${key}=${value}`;
  });

  return parameters.join ('&');
};
