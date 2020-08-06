const keyMirror = (constantsArray: string[]): { [key: string]: string } =>
  constantsArray.reduce((acc: {}, constant: string) => {
    if (constant.includes('FETCH')) {
      return {
        ...acc,
        [constant]: constant,
        [`${constant}_START`]: `${constant}_START`,
        [`${constant}_ERROR`]: `${constant}_ERROR`,
        [`${constant}_SUCCESS`]: `${constant}_SUCCESS`,
      };
    }

    return {
      ...acc,
      [constant]: constant,
    };
  }, {});

export default keyMirror;
