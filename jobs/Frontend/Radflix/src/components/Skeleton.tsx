// Skeleton.tsx
import FadeIn from "react-fade-in";
import "../CSS/Skeleton.css";

const Skeleton: React.FC<{
  //itemLimit: number | undefined;
  visible: boolean;
}> = ({ visible }) => {
  const placeholderCount = 20;
  const result = [];

  for (let i = 0; i < placeholderCount; i++) {
    result.push(<div className="skeleton-tile" key={i} />);
  }

  // Pause pulsing animation when the skeleton is hidden
  const element = document.querySelectorAll<HTMLElement>(".skeleton-tile");
  if (!visible) {
    for (let index = 0; index < element.length; index++) {
      const item: HTMLElement = element[index];
      item.style.animationPlayState = "paused";
    }
  } else {
    for (let index = 0; index < element.length; index++) {
      const item: HTMLElement = element[index];
      item.style.animationPlayState = "running";
    }
  }

  return (
    <div className="skeleton-container">
      <FadeIn
        className="skeleton-fade"
        transitionDuration={250}
        delay={5}
        visible={visible}
      >
        {result}
      </FadeIn>
    </div>
  );
};

export default Skeleton;
