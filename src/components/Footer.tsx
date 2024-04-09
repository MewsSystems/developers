import styled from "styled-components";

const FooterContainer = styled.footer`
    font-family: "Axiforma-Light", sans-serif;
    height: 100px;
    width: 100%;

    a {
        transition: all 0.25s ease-in;
        font-family: "Axiforma-Regular", sans-serif;

        &:hover {
            transition: all 0.25s ease-in;
            color: var(--btn-primary);
            text-decoration: underline;
        }
    }
`;

export const Footer = () => {
    return (
        <FooterContainer className=" bg-gray-800 text-white py-4">
            <div className="container mx-auto px-4 flex flex-col items-center">
                <div className="mb-4">
                    <span>by:</span>
                    <a
                        href="https://davidportilla.com"
                        target="_blank"
                        rel="noreferrer"
                        className="ml-1 font-bold"
                    >
                        David Portilla
                    </a>
                </div>
                <ul className="flex">
                    <li className="mr-4">
                        <a
                            href="https://www.linkedin.com/in/davidportilla/"
                            target="_blank"
                            rel="noreferrer"
                        >
                            Linkedin
                        </a>
                    </li>
                    <li>
                        <a
                            href="https://github.com/david-portilla"
                            target="_blank"
                            rel="noreferrer"
                        >
                            Github
                        </a>
                    </li>
                </ul>
            </div>
        </FooterContainer>
    );
};
