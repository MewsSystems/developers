import TestComponent from "@/components/TestComponent";
import styles from "./page.module.css";
import styled from "styled-components";

export default function Home() {
  return (
    <main className={styles.main}>
      <TestComponent />
    </main>
  );
}
