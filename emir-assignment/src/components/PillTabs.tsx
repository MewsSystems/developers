type Tab = { key: string; label: string };

export default function PillTabs({
    tabs,
    value,
    onChange,
    className = "",
}: {
    tabs: Tab[];
    value: string;
    onChange: (key: string) => void;
    className?: string;
}) {
    return (
        <div
            className={`inline-flex rounded-lg border border-white/10 bg-white/5 p-1 text-sm ${className}`}
            role="tablist"
        >
            {tabs.map((t) => {
                const active = value === t.key;
                return (
                    <button
                        key={t.key}
                        type="button"
                        role="tab"
                        aria-selected={active}
                        onClick={() => onChange(t.key)}
                        className={`px-3 py-1.5 rounded-md transition-colors ${
                            active
                                ? "bg-white/15 text-white"
                                : "text-neutral-300 hover:text-white hover:bg-white/10"
                        }`}
                    >
                        {t.label}
                    </button>
                );
            })}
        </div>
    );
}
