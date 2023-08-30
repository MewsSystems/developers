import { AppHeader } from "../components/app-header";
import { BackgroundImage } from "../components/background-image";
import { Internal } from "../components/internal";
import { SearchForm } from "../features/search-form";

export const SearchView = () => (
    <BackgroundImage url="/background.png">
        <Internal>
            <AppHeader />
            <SearchForm />
        </Internal>
    </BackgroundImage>
)