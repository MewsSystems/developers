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
            className={`flex gap-6 w-full  text-lg ${className} mb-16`}
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
                        className={`flex-1 px-4 py-2 rounded-md text-center transition-colors ${
                            active
                                ? "bg-[#00ad99] text-white"
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
