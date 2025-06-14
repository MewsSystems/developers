import { FooterWrapper } from "./style"

export const Footer: React.FC = () => {

    return (
        <FooterWrapper>
            © {new Date().getFullYear()} MovieApp — Built with{' '}
            <span role="img" aria-label="love">❤️</span> by Barbora
        </FooterWrapper>
    )
}