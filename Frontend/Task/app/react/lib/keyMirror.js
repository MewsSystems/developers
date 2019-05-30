const keyMirror = list => {
  const result = {};
  
  list.forEach(item => {
    result[item] = item;
    
    if (item.includes('FETCH') || item.includes('PROMISE') || item.includes('UPLOAD') || item.includes('DOWNLOAD') || item.includes('UPDATE')) {
      result[`${item}_START`] = `${item}_START`;
      result[`${item}_ERROR`] = `${item}_ERROR`;
      result[`${item}_SUCCESS`] = `${item}_SUCCESS`;
    }
  });
  
  return result;
};

export default keyMirror;
