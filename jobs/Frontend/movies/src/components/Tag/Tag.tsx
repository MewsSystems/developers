import React from 'react';
import './Tag.css';

interface GenreTagProps {
  name: string;
}

export const GenreTag: React.FC<GenreTagProps> = ({ name }) => {
  return <span className="genre-tag">{name}</span>;
};
