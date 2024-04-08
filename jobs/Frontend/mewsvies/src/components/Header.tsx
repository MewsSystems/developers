export const Header = () => {
    return (
        <header className="bg-gray-800 text-white py-4 top-0 left-0 w-full z-10">
            <div className="container mx-auto px-4 flex flex-col items-center">
                <h1 className="text-2xl sm:text-3xl font-bold">Mewsvies</h1>
                <h2 className="text-sm sm:text-base mt-1">
                    movies app from <span className="font-bold">themoviedb API</span>
                </h2>
            </div>
        </header>
    );
};
