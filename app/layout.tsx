import { Figtree } from "next/font/google";
import  Sidebar  from "@/components/sidebar";
import "./globals.css";

const font = Figtree({
  subsets: ["latin"],
});

export const metadata = {
  title: 'music-player',
  description: 'music-player'
}

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body
        className={`${font.className}`}
      >
        <Sidebar>
        {children}
        </Sidebar>
      </body>
    </html>
  );
}
