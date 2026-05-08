interface MovieStatusProps {
  status?: string
}

export function MovieStatus({ status }: MovieStatusProps) {
  if (!status) return null

  return (
    <div>
      <h3 className="text-lg sm:text-xl font-bold mb-2">Status</h3>
      <p className="text-sm text-muted-foreground">{status}</p>
    </div>
  )
}
