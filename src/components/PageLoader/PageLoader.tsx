import { Film } from "lucide-react"
import { Container, Spinner } from "./PageLoader.styles"

export const PageLoader = () => {
  return (
    <Container>
      <Spinner>
        <Film size={56} />
      </Spinner>
    </Container>
  )
}
