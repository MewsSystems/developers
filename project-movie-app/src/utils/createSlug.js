export const createSlug = (originalTitle) => {

    const slug = originalTitle
    .toLowerCase()
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "")
    .replace(/[^a-z0-9\s-]/g, "")   // remove punctuation (but keep hyphens)
    .trim()
    .replace(/\s+/g, "-")           // replace spaces with dashes
    .replace(/-+/g, "-");           // collapse multiple dashes

    return slug
}