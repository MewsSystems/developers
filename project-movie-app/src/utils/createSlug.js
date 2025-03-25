export const createSlug = (originalTitle) => {

    const slug = originalTitle
    .toLowerCase()
    .replace(/\s+/g, '-');

    return slug
}