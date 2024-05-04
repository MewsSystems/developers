export default function imageLoader({
  src,
  width,
}: {
  src: string;
  width: number;
}) {
  if (src) {
    return `${process.env.NEXT_PUBLIC_MOVIE_DB_API_IMAGE_BASE_URL}/w${width}${src}`;
  }

  return "next.svg";
}
