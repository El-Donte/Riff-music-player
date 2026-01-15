

import { Manrope } from "next/font/google";

import "./globals.css";
import  Sidebar  from "@/components/Sidebar";
import ModalProvider from "@/providers/ModalProvider";
import ToasterProvider from "@/providers/ToastProvider";
import UserProvider from "@/providers/UserProvide";
import getTracksByUserId from "@/actions/getTracksByUserId";
import Player from "@/components/MusicPlayer/Player";

import { Suspense, useEffect } from "react";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import toast from "react-hot-toast";
import EmailVerificationHandler from "@/components/Basic/EmailVerified";

const font = Manrope({
  subsets: ["latin"],
});

export const metadata = {
  title: 'Riff-music-player',
  description: 'Riff-music-player',
  icons: {
    icon: '/favicon.ico',
  },
}

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {

  return (
    <html lang="en">
      <body className={`${font.className}, bg-dark-violet-500/40`}>
        <UserProvider>
          <ToasterProvider/>
          <ModalProvider/>
          <Suspense fallback={<Sidebar tracks={[]} loading={true}>{children}</Sidebar>}>
            <TracksLoader>{children}</TracksLoader>
          </Suspense>
          <Player/>
          <EmailVerificationHandler/>
        </UserProvider>
      </body>
    </html>
  );
}

async function TracksLoader({ children }: { children: React.ReactNode }){
	const userTracks = await getTracksByUserId();
	return (
    <Sidebar tracks={userTracks} loading={false}>
        {children}
    </Sidebar>
  );
}
