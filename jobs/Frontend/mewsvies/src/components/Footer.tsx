export const Footer = () => {
    return (
        <footer className="bottom-0 left-0 w-full bg-gray-800 text-white py-4">
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
                            className="font-bold"
                        >
                            Linkedin
                        </a>
                    </li>
                    <li>
                        <a
                            href="https://github.com/david-portilla"
                            target="_blank"
                            rel="noreferrer"
                            className="font-bold"
                        >
                            Github
                        </a>
                    </li>
                </ul>
            </div>
        </footer>
    );
};
