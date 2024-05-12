import styled from "styled-components";

export const MainContainer = styled.main`
  padding: 20px;
  background-color: #ecf0f1; // Light grey, consistent with the rest of your app
  color: #2c3e50; // Dark grey text for readability
`;

export const Title = styled.h1`
  color: #34495e; // Dark blue for titles
  text-align: center;
  margin-bottom: 20px; // Adds space below the title
`;

export const Text = styled.p`
  font-size: 16px;
  line-height: 1.5;
  margin-bottom: 10px; // Adds space below each paragraph
`;

export const MoviePoster = styled.img`
  display: block;
  margin: 20px auto; // Centers the image horizontally
  max-width: 200px; // Fixed width for consistency
  border-radius: 8px; // Rounded corners for the image
`;
