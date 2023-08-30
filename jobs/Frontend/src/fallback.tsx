import { AppHeader } from "components/app-header";
import { BackgroundImage } from "components/background-image";
import { ErrorMessage } from "components/error-message";
import { Internal } from "components/internal";

export const Fallback = () => (
    <BackgroundImage url="/background.png">
        <Internal>
            <AppHeader />
            <ErrorMessage
                header="Something went wrong"
                message="Try refreshing the page..."
            />
        </Internal>
    </BackgroundImage>
);