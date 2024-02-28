#include "stdafx.h"


int main(int, char**)
{
    std::cout << "Hello Linux World, from " CREATION_DATE "!" << std::endl;

#if _DEBUG
    std::cout << "- Exe is built in Debug"
#  if USES_FASTBUILD
        " with FastBuild"
#  endif
        "!" << std::endl;
#endif

#if NDEBUG
    std::cout << "- Exe is built in Release"
#  if USES_FASTBUILD
        " with FastBuild"
#  endif
        "!" << std::endl;
#endif

}
