import Modal from "styled-react-modal";
import { keyframes } from "styled-components";

const openAnimation = keyframes`
  from {
    transform: scale(0.7);
    opacity: 0;
  }
  to {
    transform: scale(1);
    opacity: 1;
  }
`;

export const StyledModal = Modal.styled`
width: 80%;
max-width: 40rem;
height: auto;
max-height: 30rem; 
background-color: rgb(36, 36, 36);
color: white;
border-radius: 0.5rem;
transform-origin: center center;
animation: ${openAnimation} 0.2s ease-in-out;

@media (max-width: 768px) {
    width: 90%;
    max-width: none;
    max-height: none;
}
`;
