import { StyledFlex } from "../flex/Flex.styled"
import { StyledContainer } from "./Container.styled"
import { StyledLoader } from "./Loader.styled"

const Loader = () => {
  return (
    <StyledFlex>
      <StyledContainer>
        <StyledLoader />
      </StyledContainer>
    </StyledFlex>
  )
}
export default Loader
