import logo from "./assets/logo-movie-app.svg"
import { HeaderWrapper, LogoWrapper, LogoImage } from "./style"

export const Header: React.FC = () => {

    return (
        <HeaderWrapper role="banner">
                <LogoWrapper href="/">
                    <LogoImage src={logo} alt="Movie App" />
                </LogoWrapper>
        </HeaderWrapper>
    )
}