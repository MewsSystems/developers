import { PlaceholderBox } from "../styles/styles";

type PlaceholderProps = {
  width?: string;
  height?: string;
  text?: string;
};

const Placeholder = ({ width, height, text }: PlaceholderProps) => {
  return (
    <PlaceholderBox width={width} height={height}>
      {text || "No Image :'("}
    </PlaceholderBox>
  );
};

export default Placeholder;
