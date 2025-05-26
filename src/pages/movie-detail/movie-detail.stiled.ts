import { Img } from "@app/lib/components/image/image";
import styled from "styled-components";

export const BackdropContainer = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: -1;
  overflow: hidden;

  &::after {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.3);
  }
`;

export const BackdropImage = styled(Img)`
  width: 100%;
  height: 100%;
  object-fit: cover;
  filter: blur(8px);
  transform: scale(1.1);
`;

export const Container = styled.div`
  min-height: 100vh;
  padding: 1rem;
  max-width: 1200px;
  margin: 0 auto;
  position: relative;
  z-index: 1;
  color: white;

  @media (min-width: 768px) {
    padding: 2rem;
  }
`;

export const BackButton = styled.button`
  background: none;
  border: none;
  color: white;
  cursor: pointer;
  font-size: 1rem;
  padding: 0.5rem 0;
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  opacity: 0.8;
  transition: opacity 0.3s ease;

  &:hover {
    opacity: 1;
  }
`;

export const MovieGrid = styled.div`
  display: grid;
  grid-template-columns: 1fr;
  gap: 1.5rem;
  background: rgba(40, 40, 40, 0.4);
  padding: 1.5rem;
  border-radius: 12px;
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);

  @media (min-width: 768px) {
    grid-template-columns: 300px 1fr;
    gap: 2rem;
    padding: 2rem;
  }
`;

export const Poster = styled.img`
  width: 100%;
  max-width: 300px;
  margin: 0 auto;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3);

  @media (min-width: 768px) {
    margin: 0;
  }
`;

export const MovieInfo = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1rem;
`;

export const Title = styled.h1`
  margin: 0;
  font-size: 1.75rem;
  color: white;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.5);
  text-align: center;

  @media (min-width: 768px) {
    font-size: 2.5rem;
    text-align: left;
  }
`;

export const MovieDetails = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  justify-content: center;
  margin-bottom: 0.5rem;

  @media (min-width: 768px) {
    justify-content: flex-start;
  }
`;

export const DetailItem = styled.div`
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: rgba(255, 255, 255, 0.9);
  font-size: 0.9rem;
  background: rgba(255, 255, 255, 0.1);
  padding: 0.5rem 1rem;
  border-radius: 20px;
  backdrop-filter: blur(4px);

  @media (min-width: 768px) {
    font-size: 1rem;
  }
`;

export const ReleaseDate = styled.p`
  color: rgba(255, 255, 255, 0.9);
  margin: 0;
  font-size: 1rem;
  text-align: center;

  @media (min-width: 768px) {
    font-size: 1.1rem;
    text-align: left;
  }
`;

export const Overview = styled.p`
  color: rgba(255, 255, 255, 0.9);
  line-height: 1.6;
  margin: 0;
  font-size: 1rem;
  text-align: center;

  @media (min-width: 768px) {
    font-size: 1.1rem;
    text-align: left;
  }
`;

export const Rating = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  color: rgba(255, 255, 255, 0.9);
  font-size: 1rem;

  @media (min-width: 768px) {
    justify-content: flex-start;
    font-size: 1.1rem;
  }
`;