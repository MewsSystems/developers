import Image from "next/image"

type MovieCardPosterProps = {
  title: string
  posterPath: string | null
}
const baseUrl = "https://image.tmdb.org/t/p/w200/"

const FallbackSVG = () => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 200 300"
    width="200"
    height="300"
  >
    <rect width="200" height="300" fill="#f0f0f0" />
    <path d="M0 0h200v300H0z" fill="#e0e0e0" />
    <path d="M20 20h160v260H20z" fill="#d0d0d0" />
    <path d="M0 0h200v40H0z M0 260h200v40H0z" fill="#c0c0c0" />
    <path d="M10 10h180v20H10z M10 270h180v20H10z" fill="#b0b0b0" />
    <text
      x="100"
      y="160"
      fontFamily="Arial, sans-serif"
      fontSize="24"
      textAnchor="middle"
      fill="#808080"
    >
      No Image
    </text>
    <path d="M60 190 l40 -40 l40 40 l-40 40 z" fill="#a0a0a0" />
    <circle cx="100" cy="190" r="20" fill="#909090" />
  </svg>
)

export const MovieCardPoster = ({
  title,
  posterPath,
}: MovieCardPosterProps) => {
  if (!posterPath) {
    return <FallbackSVG />
  }

  return (
    <Image
      src={`${baseUrl}${posterPath}`}
      alt={title}
      width={300}
      height={300}
      className="rounded-md"
    />
  )
}
