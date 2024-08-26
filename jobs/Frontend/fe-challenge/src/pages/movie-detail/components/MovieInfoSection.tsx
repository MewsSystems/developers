import React from 'react';

interface MovieInfoSectionProps {
  title: string;
  children: React.ReactNode;
}

const MovieInfoSection = ({ title, children }: MovieInfoSectionProps) => {
  return (
    <section>
      <div className="font-semibold mt-4 mb-2">{title}</div>
      <div>{children}</div>
    </section>
  );
};

export default MovieInfoSection;
