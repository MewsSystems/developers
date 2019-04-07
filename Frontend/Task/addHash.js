/**
 * Adds hash to file depending on NODE_ENV variable value
 * @param template
 * @param hash
 * @returns {*}
 */
function addHash(template, hash) {
  const NODE_ENV = process.env.NODE_ENV || 'development';

  return NODE_ENV === 'production'
    ? template.replace(/\.[^.]+$/, `.[${hash}]$&`) : template;
}

module.exports = addHash;
