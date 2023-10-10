import "./Alert.css";

const Alert: React.FC<{ message: string }> = ({ message }) => {
  return (
    <div className={`alertMessage ${message ? "" : "hidden"}`}>
      <p>{message}</p>
    </div>
  );
};

export default Alert;
