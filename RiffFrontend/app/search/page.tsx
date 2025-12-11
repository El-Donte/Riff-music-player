import getTracksByTitle from "@/actions/getTracksByTitle";
import Header from "@/components/Header";
import SearchInput from "@/components/SearchInput";
import SearchContent from "./components/SearchContent";

import { Suspense } from "react";


interface SearchProps {
  searchParams: {
    title: string;
  }
};

export const revalidate = 0;

async function TracksLoader({ searchParams }: SearchProps) {
	const tracks = await getTracksByTitle(searchParams.title);
	return <SearchContent tracks={tracks} loading={false} />;
}

const Search = async ({ searchParams }: SearchProps) => {
  return (
    <div
      className="
        bg-neutral-900
        rounded-lg
        h-full
        w-full
        overflow-hidden
        overflow-y-auto
      "
    >
      <Header className="from-bg-neutral-900">
        <div className="mb-2 flex flex-col gap-y-6">
          <h1 className="text-white text-3xl font-semibold">
            Поиск
          </h1>
          <SearchInput />
        </div>
      </Header>
      <Suspense fallback={<SearchContent tracks={[]} loading={true} />}>
				<TracksLoader searchParams={searchParams}/>
			</Suspense>
    </div>
  );
};

export default Search;

