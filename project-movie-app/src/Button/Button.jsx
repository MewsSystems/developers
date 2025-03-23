import "./button.style.css";

export const Button = ({ label = "Show more", handleLoadMore}) => {

    return (
        <button 
            className="button"
            onClick={handleLoadMore}
        >
            {label}
        </button>
    )
}