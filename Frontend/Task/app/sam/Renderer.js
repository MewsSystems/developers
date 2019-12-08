import ReactDOM from "react-dom";
import { Main } from "../components/Main";

export const render = (state) => {
    console.log(state);
    ReactDOM.render(
        Main(state),
        document.getElementById("exchange-rate-client")
    );
}