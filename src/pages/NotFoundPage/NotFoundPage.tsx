import { ArrowLeft, Film } from "lucide-react"
import { ROUTES } from "@/constants/routes"
import {
  BackButton,
  MovieIcon,
  NotFoundContainer,
  NotFoundMessage,
  NotFoundSubtitle,
  NotFoundTitle,
} from "./NotFoundPage.styles"

const NotFoundPage = () => {
  return (
    <NotFoundContainer>
      <MovieIcon>
        <Film size={64} />
      </MovieIcon>
      <NotFoundTitle>404</NotFoundTitle>
      <NotFoundSubtitle>Page Not Found</NotFoundSubtitle>
      <NotFoundMessage>
        Oops! The page you're looking for doesn't exist. It might have been moved, deleted, or you
        entered the wrong URL.
      </NotFoundMessage>
      <BackButton to={ROUTES.HOME}>
        <ArrowLeft size={16} />
        Back to Movie Search
      </BackButton>
    </NotFoundContainer>
  )
}

export default NotFoundPage
