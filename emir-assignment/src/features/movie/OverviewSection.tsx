import { formatCurrency } from "./utils";

export default function OverviewSection(props: {
    posterUrl: string;
    overview: string;
    details: {
        genres?: string;
        languages?: string;
        status?: string;
        runtime?: string;
        releaseDate?: string;
        budget?: number;
        revenue?: number;
        companies?: string[];
    };
}) {
    const { posterUrl, overview, details } = props;
    return (
        <div className="grid gap-4 lg:gap-20 grid-cols-12">
            <div className="col-span-6 sm:col-span-4 md:col-span-3 overflow-hidden ">
                {posterUrl ? (
                    <img
                        src={posterUrl}
                        alt=""
                        className="w-full object-cover rounded-xl border border-white/10 bg-white/5"
                        loading="lazy"
                    />
                ) : (
                    <div className="" />
                )}
            </div>
            <div className="col-span-12 sm:col-span-8 md:col-span-9 space-y-4">
                <h2 className="text-lg font-semibold">Storyline</h2>
                {overview ? (
                    <p className="text-neutral-200">{overview}</p>
                ) : (
                    <p className="text-neutral-400">No overview available.</p>
                )}

                <h3 className="mt-4 text-sm font-semibold text-neutral-300">
                    Details
                </h3>
                <dl className="grid grid-cols-1 gap-x-6 gap-y-3 sm:grid-cols-2">
                    {details.genres && (
                        <DetailRow term="Genres" value={details.genres} />
                    )}
                    {details.languages && (
                        <DetailRow term="Languages" value={details.languages} />
                    )}
                    {details.status && (
                        <DetailRow term="Status" value={details.status} />
                    )}
                    {details.runtime && (
                        <DetailRow term="Runtime" value={details.runtime} />
                    )}
                    {details.releaseDate && (
                        <DetailRow
                            term="Release date"
                            value={details.releaseDate}
                        />
                    )}
                    {details.budget ? (
                        <DetailRow
                            term="Budget"
                            value={formatCurrency(details.budget)}
                        />
                    ) : null}
                    {details.revenue ? (
                        <DetailRow
                            term="Revenue"
                            value={formatCurrency(details.revenue)}
                        />
                    ) : null}
                    {details.companies && details.companies.length > 0 ? (
                        <DetailRow
                            term="Production"
                            value={details.companies.slice(0, 6).join(", ")}
                        />
                    ) : null}
                </dl>
            </div>
        </div>
    );
}

function DetailRow({ term, value }: { term: string; value: string }) {
    return (
        <div className="grid grid-cols-[120px,1fr] items-baseline gap-2">
            <dt className="text-xs uppercase tracking-wide text-neutral-400">
                {term}
            </dt>
            <dd className="text-sm text-neutral-200">{value}</dd>
        </div>
    );
}
