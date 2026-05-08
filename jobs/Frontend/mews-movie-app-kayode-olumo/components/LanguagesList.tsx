interface Language {
  iso_639_1: string
  name: string
}

interface LanguagesListProps {
  languages: Language[]
}

export function LanguagesList({ languages }: LanguagesListProps) {
  if (!languages || languages.length === 0) return null

  return (
    <div>
      <h3 className="text-lg sm:text-xl font-bold mb-2">Languages</h3>
      <p className="text-sm text-muted-foreground">
        {languages.map((lang) => lang.name).join(", ")}
      </p>
    </div>
  )
}
