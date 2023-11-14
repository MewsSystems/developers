import { FC } from "react";
import { Autoplay } from "swiper/modules";
import { Swiper, SwiperSlide } from "swiper/react";
import { Movie } from "@/types/movie";
import { HeroSlideItem } from "./hero-slide-item";

type Props = {
  movies: Movie[];
};

export const HeroSlider: FC<Props> = (props) => {
  const { movies } = useHeroSlider(props);

  const renderMovie = (movie: Movie) => (
    <SwiperSlide key={movie.id}>
      {({ isActive }) => <HeroSlideItem movie={movie} isActive={isActive} />}
    </SwiperSlide>
  );

  return (
    <Swiper
      modules={[Autoplay]}
      grabCursor={true}
      spaceBetween={0}
      slidesPerView={1}
      autoplay={{ delay: 7500 }}
      style={{ height: "100vh" }}
    >
      {movies.map(renderMovie)}
    </Swiper>
  );
};

function useHeroSlider({ movies }: Props) {
  return { movies };
}
