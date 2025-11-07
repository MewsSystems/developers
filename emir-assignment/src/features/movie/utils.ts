export function yearFrom(date?: string) {
    return (date && date.split("-")[0]) || "";
}

export function formatRuntime(min?: number) {
    if (!min || min <= 0) return "";
    const h = Math.floor(min / 60);
    const m = min % 60;
    return `${h}h ${m}m`;
}

export function joinNames<T extends { name: string }>(arr?: T[], max = 4) {
    if (!arr || arr.length === 0) return "";
    return arr
        .slice(0, max)
        .map((g) => g.name)
        .join(", ");
}

export function formatCurrency(n?: number) {
    if (!n || n <= 0) return "";
    return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
        notation: "compact",
    }).format(n);
}
