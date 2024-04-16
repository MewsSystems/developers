import { createRootRouteWithContext, Outlet } from "@tanstack/react-router";
import { TanStackRouterDevtools } from "@tanstack/router-devtools";
import { QueryClient } from "@tanstack/react-query";
import { createGlobalStyle } from "styled-components";

const GlobalStyle = createGlobalStyle`
* {
  min-width: 0;
  font: inherit; 
} 

*, *:: before, *:: after {
  box-sizing: border-box;
} 

img, video, svg {
  display: block;
  height: auto;
  max-width: 100%; 
} 

body {
  margin: 0;
  min-height: 100dvh; 
  background-color: #efefef;
  max-width: 72rem;
  margin: 0px auto;
} 

h1, h2, h3, h4, h5, h6 {
  text-wrap: balance;
} 

p {
  text-wrap: pretty;
}
`;

export const Route = createRootRouteWithContext<{
  queryClient: QueryClient;
}>()({
  component: () => (
    <>
      <Outlet />
      <TanStackRouterDevtools />
      <GlobalStyle />
    </>
  ),
});
