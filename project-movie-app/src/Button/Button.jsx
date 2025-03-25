import "./button.style.css";

export const Button = ({ label, handleLoadMore}) => {

    return (
        <button 
            className="button"
            onClick={handleLoadMore}
        >
            {label}
        </button>
    )
}