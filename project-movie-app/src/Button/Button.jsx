import "./button.style.css";

export const Button = ({ label = "Show more"}) => {

    return (
        <button className="button">
            {label}
        </button>
    )
}