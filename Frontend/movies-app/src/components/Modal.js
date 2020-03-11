import React, { useEffect } from 'react';
import { createPortal } from 'react-dom';
import styled from 'styled-components';

const Overlay = styled.div`
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background-color: ${props => props.theme.background.primary};
  opacity: 0.9;
`;

const Content = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  color: ${props => props.theme.text.primary};
`;

const Modal = ({ children, isOpen, onClose }) => {
  const modalRoot = document.body;
  const el = document.createElement('div');
  useEffect(() => {
    modalRoot.appendChild(el);
    return () => modalRoot.removeChild(el);
  }, [el, modalRoot]);

  return isOpen
    ? createPortal(
        <Overlay onClick={onClose}>
          <Content>{children}</Content>
        </Overlay>,
        el
      )
    : null;
};

export default Modal;
